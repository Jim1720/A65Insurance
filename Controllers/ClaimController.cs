using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using A65Insurance.Models;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;

namespace A65Insurance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimController : ControllerBase
    {
        private readonly A45InsuranceContext _context;

        public ClaimController(A45InsuranceContext context)
        {
            _context = context;
        }

        // GET: api/Claim
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Claim>>> GetClaim()
        {
            return await _context.Claim.ToListAsync();
        }

        // GET: api/Claim/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Claim>> GetClaim(String id)
        {

            // remove lead trail quotes added by API call.
            // this only happed from blazor not C# mvc so check if needed
            // by asking if "C" in postion one as expected...

            var firstCharacter = id.Substring(0, 1);
            var needToRemoveQuotes = firstCharacter != "C";
            
            if(needToRemoveQuotes)
            {
              int shorterLength = id.Length - 2;
              id = id.Substring(1, shorterLength);
            }
          

            Claim claim = await _context.Claim.FirstOrDefaultAsync<Claim>
               (clm => clm.ClaimIdNumber == id);

            if (claim == null)
            {
                return NotFound();
            }

            return Ok(claim);
        }

        // GET: api/Claim/History/{id}
        [HttpGet("/History/{id}")]
        public async Task<ActionResult<List<Claim>>> History(string id)
        {
            var historyClaims =
                from c in _context.Claim
                where id == c.CustomerId
                select c;

        

            List<Claim> history = await historyClaims.ToListAsync();

            if (history.Count == 0)
            {
                return NotFound();
            }

            return history;
        }

        [HttpPut()]
        [Route("/api/StampAdjustedClaim/")]
        public async Task<ActionResult<string>> StampAdjustedClaim(
            [Bind("AdjustedClaimId,AdjustingClaimId,DateAdjusted,AppAdjusting")] 
            StampData stampData)
        { 

                var claimToStampLinq = from c in _context.Claim
                                where c.ClaimIdNumber == stampData.AdjustedClaimId
                                select c;

                foreach (Claim item in claimToStampLinq)
                {
                    item.AdjustingClaimId = stampData.AdjustingClaimId;
                    item.AdjustedDate = stampData.DateAdjusted;
                    item.AppAdjusting = stampData.AppAdjusting;
                    item.ClaimStatus = "Adjusted";
                    _context.Update(item);
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (SystemException ex)
                {
                    string msg = ex.Message.ToString();
                    return "not ok" + msg;
                }

                return "OK"; 
         
        }

        [HttpPut()]
        [Route("/api/PayClaim/")]
        public async Task<ActionResult<string>> PayClaim(
                    [Bind("ClaimId,Amount,Date")
                    ]PayData payData)
        {      

            DateTime paymentDate = DateTime.Now;

            var id = payData.ClaimId;

            // remove lead trail quotes added by API call.
            // this only happed from blazor not C# mvc so check if needed
            // by asking if "C" in postion one as expected...

            var firstCharacter = id.Substring(0, 1);
            var needToRemoveQuotes = firstCharacter != "C";

            if (needToRemoveQuotes)
            {
                int shorterLength = id.Length - 2;
                id = id.Substring(1, shorterLength);
            }


            var linqClaim = from c in _context.Claim
                                where c.ClaimIdNumber == id
                                select c;

                foreach (Claim item in linqClaim)
                {
                    item.PaymentAmount = (decimal)payData.Amount;
                    item.BalanceOwed -= item.PaymentAmount;
                    item.ClaimStatus = "Paid";
                    _ = DateTime.TryParse(payData.Date, out paymentDate);
                    item.PaymentDate = paymentDate;
                    _context.Update(item);
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (SystemException ex)
                {
                    string msg = ex.Message.ToString();
                    return "not ok" + msg;
                }

                return "OK";
         
        }



        // PUT: api/Claim/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClaim(int id, Claim claim)
        {
            if (id != claim.Id)
            {
                return BadRequest();
            }

            _context.Entry(claim).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClaimExists(id))
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

        // POST: api/Claim
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Claim>> PostClaim([Bind("ClaimIdNumber," +
            "ClaimDescription,CustomerId,PlanId,PatientFirst,PatientLast," +
            "Diagnosis1,Diagnosis2," + 
            "Procedure1,Procedure2,Procedure3," +
            "Physician,Clinic,DateService,Service,ServiceItem,"  +
            "Location,TotalCharge,PaymentAmount,PaymentDate",
            "PaymentPlan,DateAdded,AdjustedClaimId,AdjustingClaimId," +
            "AdjustedDate,AppAdjusting,ClaimStatus,Referral,",
            "PaymentAction,ClaimType,DateConfine,DateRelease,",
            "ToothNumber,Eyeware,DrugName")]Claim claim)
        {

            claim.ClaimIdNumber = claim.ClaimIdNumber.Trim();

            _context.Claim.Add(claim);
            await _context.SaveChangesAsync();

            return Ok(claim);

           // return CreatedAtAction("GetClaim", new { id = claim.Id }, claim); 

        }

        // DELETE: api/Claim/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Claim>> DeleteClaim(int id)
        {
            var claim = await _context.Claim.FindAsync(id);
            if (claim == null)
            {
                return NotFound();
            }

            _context.Claim.Remove(claim);
            await _context.SaveChangesAsync();

            return claim;
        }

        private bool ClaimExists(int id)
        {
            return _context.Claim.Any(e => e.Id == id);
        }
    }
}
