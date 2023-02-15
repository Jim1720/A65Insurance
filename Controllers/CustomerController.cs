using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using System.Text.Json;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using A65Insurance.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;
using System.Net.Http; 
using Microsoft.Extensions.Caching.Memory;

// Release 2 - reads for plans and services; /UpdatePlan api routine added. 

namespace A65Insurance.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]  
    public class CustomerController : ControllerBase
    {
        private readonly A45InsuranceContext _context; 
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache memoryCache;

        public bool HttpOnly { get; private set; }

        protected string getVar(string key)
        {
            var value = _configuration.GetValue<string>(key, "");
            if (value == null)
            {
                var msg = "CustomerController: no environment data for " + key;
                _ = new InvalidOperationException(msg);
            }
            return value;

        }

        public CustomerController(A45InsuranceContext context, IConfiguration configuration, IMemoryCache mc)
        {
            _context = context;
            _configuration = configuration;
            memoryCache = mc; 
        } 
       

        // GET: api/Customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomer()
        {
            return await _context.Customer.ToListAsync();
        } 
        // GET: api/Customer/name
        [HttpGet("{name}")]
        public async Task<ActionResult<Customer>> GetCustomer(string name)
        {

            var customer = await _context.Customer.FirstOrDefaultAsync<Customer>
                (cust => cust.CustId == name);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer); 

        }

        // GET: api/signin/name
        [HttpGet("/api/Signin/{name}")] 
        public async Task<ActionResult<Customer>> Signin(string name)
        {
            // same a get customer that is used for reg dup check but this on
            // returns a token.

            var customer = await _context.Customer.FirstOrDefaultAsync<Customer>
                (cust => cust.CustId == name);

            if (customer == null)
            {
                return NotFound();
            }

            ReturnToken(customer.CustId); 

            return Ok(customer);

        }

        private void ReturnToken(string custId)
        {
            

            var value = string.Empty;
            var rand = new Random();
            for (int i = 0; i <= 19; i++)
            {
                value += rand.Next(10);
            }  
             

            Response.Cookies.Append("A65TOKEN", value, new CookieOptions()
            {
                HttpOnly = false
            }); 

            var minutes = 14;
            var expirationSeconds = minutes * 60; 

            var options =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(expirationSeconds));

            memoryCache.Set(custId.Trim(), value, options);

        }

        // GET: api/signin70/name
        [HttpGet("/api/Signin70/{name}")]
        public async Task<ActionResult<Customer>> Signin70(string name)
        {
            // same a get customer that is used for reg dup check but this on
            // returns a token. used by blazor a70 only.

            var customer = await _context.Customer.FirstOrDefaultAsync<Customer>
                (cust => cust.CustId == name);

            if (customer == null)
            {
                return NotFound();
            }

            // for blazor pass token in 'encrypted' customer field.
            customer.Encrypted = FieldToken(customer.CustId);

            return Ok(customer);

        }


        private string FieldToken(string custId)
        {


            var value = string.Empty;
            var rand = new Random();
            for (int i = 0; i <= 19; i++)
            {
                value += rand.Next(10);
            } 

            var minutes = 14;
            var expirationSeconds = minutes * 60;

            var options =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(expirationSeconds));

            memoryCache.Set(custId.Trim(), value, options);

            return value;

        }

        // GET: api/Reference/{option}
        [HttpGet("/api/Reference/{id}")] 
        public  ActionResult<string> ReferenceData(string id)
        {
            var option = id;

            if(option == "PromotionCode")
            {
                return Ok(getVar("PromotionCode"));
            }

            if(option == "Adm")
            {
                return Ok(getVar("AdmId") + "," + getVar("AdmPassword"));
            } 

            return "UnusedOption";

        }

        class PlanEntry
        {
            public string PlanName { get; set; }
            public string PlanLit { get; set; }
            public string Percent { get; set; }
        }

        // GET: api/ReadPlans   
        [HttpGet]
        [Route("/api/readPlans/")] 
        public async Task<ActionResult<string>> ReadPlans()
        {
             

            try
            {

                //TODO: cf conn string config env var .
                var plans = await _context.Plan.ToListAsync(); 

                List<PlanEntry> list = new List<PlanEntry>();
                foreach (var p in plans)
                { 
                    var entry = new PlanEntry { PlanName = p.PlanName, PlanLit = p.PlanLiteral, Percent = p.Percent };
                    
                    list.Add(entry);
                }

                //Newtonsoft.Json.JsonConvert.SerializeObject(plans)
                //System.Text.Json.JsonSerializer.Serialize(object,inputtype)

                String json = Newtonsoft.Json.JsonConvert.SerializeObject(plans);

                //Console.WriteLine("read plans- count is:" + list.Count);

                return Ok(json);

                
            } 
            catch(System.IO.FileNotFoundException ex)
            {
                return NotFound(ex.Message.ToString());
            }
            catch (System.Exception ex)
            {
                return NotFound(ex.Message.ToString());
            } 

        }

        class ServiceEntry
        {
            public string ServiceName { get; set; }
            public string ClaimType { get; set; } 
            public string ClaimTypeLiteral { get; set; }
            public string Cost { get; set; }
        }


        // GET: api/Services
        [HttpGet]
        [Route("/api/readServices/")]
        public async  Task<ActionResult<string>>ReadServices()
        {
            try
            {

                //TODO: cf conn string config env var .
                var services = await _context.Service.ToListAsync();

                List<ServiceEntry> list = new List<ServiceEntry>();
                foreach (var s in services)
                {
                    var entry = new ServiceEntry { ServiceName = s.ServiceName, 
                                              ClaimType = s.ClaimType,
                                              ClaimTypeLiteral = s.ClaimTypeLiteral,
                                              Cost = s.Cost.ToString() };

                    list.Add(entry);
                }

                String json = Newtonsoft.Json.JsonConvert.SerializeObject(list);

                //Console.WriteLine("read services - count is:" + list.Count);

                return Ok(json);


            }
            catch (System.IO.FileNotFoundException ex)
            {
                return NotFound(ex.Message.ToString());
            }
            catch (System.Exception ex)
            {
                return NotFound(ex.Message.ToString());
            }

        }

          

        [HttpPut()]
        [Route("/api/ChangePassword/")]
        public async Task<string> ResetPassword(
               [Bind("CustomerId,NewPassword")]PasswordChanger passwordChanger)
        {

            

            var linqCustomer = from c in _context.Customer
                                   where c.CustId == passwordChanger.CustomerId
                                   select c;

                foreach (Customer item in linqCustomer)
                {
                    item.CustPassword = passwordChanger.NewPassword;
                    _context.Update(item);
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (SystemException ex)
                {
                    string msg = ex.Message.ToString();
                    return "Bad result " + msg;
                }

                return "OK";
          
        }

        [HttpPut("")] //here
        [Route("/api/UpdateCustomerPlan/")]
        public ActionResult<HttpResponse> UpdatePlan([Bind("CustomerId, PlanName")] PlanInput plan) 
        {

            if (NoTokenPresent(HttpContext, plan.CustomerId))
            {
                return BadRequest();
            }

            // update custome plan.

            var linqCustomer = from customer in _context.Customer
                               where customer.CustId == plan.CustomerId
                               select customer;

            foreach(var cust in linqCustomer)
            {
                cust.CustPlan = plan.PlanName;
                _context.Update(cust);
            }

            _context.SaveChanges();

            return Ok("OK");


        }


        [HttpPut("")]
        [Route("/api/ResetCustomer/")]
        public  async  Task<ActionResult<string>> ResetCustomerId(
             [Bind("CustomerId,NewCustomerId")]
             CustomerResetter reset)
        {

          
            var findCurrentCustomer = from customer in _context.Customer
                                   where customer.CustId == reset.CustomerId
                                   select customer;
                bool exists = false;
                foreach (Customer item in findCurrentCustomer)
                {
                    exists = true;
                }
                if (!exists)
                {
                   return "Customer Id does not exist to be reset";
                }

               var findNewCustomer = from customer in _context.Customer
                                   where customer.CustId == reset.NewCustomerId
                                   select customer;
                 exists = false;
                foreach (Customer item in findNewCustomer)
                {
                    exists = true;
                }
                if (exists)
                {
                    return "New Customer Id already exists";
                }

                // now do the operation. 

                var customerToResetLinq = from customer in _context.Customer
                                   where customer.CustId == reset.CustomerId
                                   select customer;

                Customer newCustomer = new Customer();
                Customer currentCustomer = null;

                foreach (Customer customer in customerToResetLinq)
                {
                    currentCustomer = customer;
                    _context.Remove(currentCustomer);
                    newCustomer.CustId = reset.NewCustomerId;
                    newCustomer.CustPassword = currentCustomer.CustPassword;
                    newCustomer.CustFirst = currentCustomer.CustFirst;
                    newCustomer.CustLast = currentCustomer.CustLast;
                    newCustomer.CustMiddle = currentCustomer.CustMiddle;
                    newCustomer.CustPhone = currentCustomer.CustPhone;
                    newCustomer.CustEmail = currentCustomer.CustEmail;
                    newCustomer.CustAddr1 = currentCustomer.CustAddr1;
                    newCustomer.CustAddr2 = currentCustomer.CustAddr2;
                    newCustomer.CustCity = currentCustomer.CustCity;
                    newCustomer.CustState = currentCustomer.CustState;
                    newCustomer.CustZip = currentCustomer.CustZip;
                    newCustomer.Encrypted = currentCustomer.Encrypted;
                    newCustomer.CustBirthDate = currentCustomer.CustBirthDate;
                    newCustomer.CustGender = currentCustomer.CustGender;
                    newCustomer.ExtendColors = currentCustomer.ExtendColors;
                    newCustomer.CustPlan = currentCustomer.CustPlan;
                    newCustomer.ClaimCount = currentCustomer.ClaimCount;
                    newCustomer.PromotionCode = currentCustomer.PromotionCode;
                    _context.Add(newCustomer);

                }

                // reset custId's on claims. 
                var claimsLinq = from c in _context.Claim
                                 where c.CustomerId == reset.CustomerId
                                 select c;

                foreach (var claim in claimsLinq)
                {
                    claim.CustomerId = reset.NewCustomerId;
                    _context.Update(claim);
                }


                try
                {
                    int v = await _context.SaveChangesAsync();
                }
                catch (SystemException ex)
                {
                    string msg = ex.Message.ToString();
                    return "Bad result " + msg;
                }

                return  "OK";

           
        }

        // PUT: api/Customer/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("")]
        [Route("/api/UpdateCustomer/")]


        private Boolean NoTokenPresent(HttpContext httpContext, string customerId)
        { 

            // true if no token present - works a60 and a70 ok. A65TOKEN,a65token seen in display...
            if(HttpContext.Request.Headers["A65TOKEN"].Count == 0)
            {
                // no token found 
                return false;
            }

            // verify proper value of token.  
             

            Microsoft.Extensions.Primitives.StringValues inputValue; 

            if (!httpContext.Request.Headers.TryGetValue("A65TOKEN", out inputValue))
            {
                return false;  
            }

            string inputTokenValue = "";

            if(!TryExtractBetween(inputValue, "A65TOKEN=",";", out inputTokenValue))
            {
                return false;
            }
           
            string cacheValue = "";

            if (!memoryCache.TryGetValue<string>(customerId, out cacheValue))
            {

                return false;
            }

            string tokenCachedValue = cacheValue.ToString();

            return inputTokenValue != tokenCachedValue;

        }

        private bool TryExtractBetween(string input, string first, string second, out string result)
        {

            // get characters betwen 1st and 2nd values. 
            result = "";

            int start = input.IndexOf(first);
            if (start == -1)
            {
                return false;
            }

            start += first.Length;
            int end = input.IndexOf(second, start);

            if(end == -1)
            {
                return false;
            }

            if (end > start)
            {
                result = input.Substring(start, end - start);
            }

            return true;

        }

        [HttpPut("")]  
        [Route("/api/UpdateCustomer/")]
        public async Task<ActionResult<HttpResponse>> UpdateCustomer([FromBody] Customer customer)
        {
            
            if (NoTokenPresent(HttpContext,customer.CustId))  
            {
                return BadRequest();
            }  

            Customer cust = new Customer();
 

                var linqCustomer = from c in _context.Customer
                                   where c.CustId == customer.CustId
                                   select c;

                var dupCount = 0;
                foreach (Customer item in linqCustomer)
                {
                    dupCount++;
                    item.CustId = customer.CustId;
                    item.CustPassword = customer.CustPassword;
                    item.CustFirst = customer.CustFirst;
                    item.CustLast = customer.CustLast;
                    item.CustMiddle = customer.CustMiddle;
                    item.CustPhone = customer.CustPhone;
                    item.CustEmail = customer.CustEmail;
                    item.CustAddr1 = customer.CustAddr1;
                    item.CustAddr2 = customer.CustAddr2;
                    item.CustCity = customer.CustCity;
                    item.CustState = customer.CustState;
                    item.CustZip = customer.CustZip;
                    item.Encrypted = customer.Encrypted;
                    item.CustBirthDate = customer.CustBirthDate;
                    item.CustGender = customer.CustGender;
                    item.ExtendColors = customer.ExtendColors;
                    item.CustPlan = customer.CustPlan;
                    item.ClaimCount = customer.ClaimCount;
                    item.PromotionCode = customer.PromotionCode;
                    _context.Update(item);
                }
                if (dupCount == 0) { return Ok("Not Found"); };
                if (dupCount > 1) { return Ok("Duplicate");  };

                try
                {
                    await _context.SaveChangesAsync();

                }
                catch (System.Exception)
                {
                    return BadRequest("Server Error");
                }

                return Ok("Ok");
                
           
        }

       

        [HttpPut]
        [Route("/api/AddClaimCount/{id}")]
        public async Task<int> AddClaimCount(string id)
        {
                
            // return -1 if bad ; new claim count when update is ok.

            var linqCustomer = from c in _context.Customer
                               where c.CustId == id
                               select c;
            Customer cust = new Customer();
            var dupCount = 0;
            int returnCount = 0;
            foreach (Customer item in linqCustomer)
            {
                dupCount++;
                int claimCount = 0;
                if (Int32.TryParse(item.ClaimCount, out claimCount))
                {
                    claimCount++;
                    item.ClaimCount = claimCount.ToString();
                    _context.Update(item);
                    returnCount = claimCount;
                }
            }
            var NotFound = -2;
            var Duplicate = -3;
            var InternalError = -5;
            if (dupCount == 0) { return NotFound; };
            if (dupCount > 1) { return Duplicate; };

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (SystemException)
            {
                return InternalError;
            }
            return returnCount;

        }


        // PUT: api/Customer/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Customer
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            Customer addCustomer = new Customer();

         /*   if (await TryUpdateModelAsync<Customer>(
                    addCustomer,
                    "AddCustomer",
                    s => s.CustId,
                    s => s.CustPassword,
                    s => s.CustFirst,
                    s => s.CustMiddle,
                    s => s.CustLast,
                    s => s.CustGender,
                    s => s.CustPhone,
                    s => s.CustEmail,
                    s => s.CustCity,
                    s => s.CustState,
                    s => s.CustZip,
                    s => s.CustBirthDate,
                    s => s.Encrypted,
                    s => s.CustPlan,
                    s => s.PromotionCode,
                    s => s.AppId,
                    s => s.ExtendColors,
                    s => s.ClaimCount))

            { */


                _context.Customer.Add(customer);
                await _context.SaveChangesAsync(); 
                 
                // Currently, post is ONLY called for registration, after authentication
                // therefore produce token.

                ReturnToken(customer.CustId);

                return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);

           /* }
            return null; */
        }


        [HttpPost]
        [Route("/api/PostCustomer70/")]
        public async Task<ActionResult<Customer>> PostCustomer70(Customer customer)
        {
            Customer addCustomer = new Customer();
 


                _context.Customer.Add(customer);
                await _context.SaveChangesAsync();

                // Currently, post is ONLY called for registration, after authentication
                // therefore produce token.

                var token =  FieldToken(customer.CustId);

                return Ok(token);

            
        }
        // DELETE: api/Customer/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();

            return customer;
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.Id == id);
        }
    }
     
}
