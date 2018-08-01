using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebServiceTrial1.Models;
using System.Xml.Linq;
using Elmah;

namespace WebServiceTrial1.Controllers
{
    /// <summary>
    ///Expense Controller
    /// </summary>
    public class ExpenseController : ApiController
    {
        //TODO: Accepts a block of text
        //TODO:Extracts the relevant data
        //TODO: Makes it available to the service’s client. 
        //Failure Conditions: Opening tags that have no corresponding closing tag. In this case the whole message should
        // be rejected.
        // Missing<total>.In this case the whole message should be rejected.
        // Missing<cost_centre>.In this case the ‘cost centre’ field in the output should be defaulted
        //to ‘UNKNOWN’. 

        /// <summary>
        /// List of Expense objects
        /// </summary>
        public List<Expense> expenses = new List<Expense>();

        List<Error> errorLog = new List<Error>();
        // GET api/expense  
        /// <summary>
        /// This method gets all the expense records in the database
        /// </summary>
        /// <returns>expense record as XML</returns>        
        public HttpResponseMessage Get()
        {
            expenses.Add(Load("<expense><cost_centre>DEV001</cost_centre><total>890.55</total><payment_method>personalcard</payment_method><date>23/07/2018</date><description>testexpenserecord</description></expense>"));
            if (expenses != null)
            {
                if (expenses.Count() > 0)
                    return Request.CreateResponse(HttpStatusCode.OK, expenses.ToArray());
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No records exist");
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, "No records exist");
        }

        // GET api/expense/5
        /// <summary>
        /// This method gets all the expense record for the id requested
        /// </summary>
        /// <returns>expense record as XML</returns>   
        public HttpResponseMessage Get(int id)
        {
            expenses.Add(Load("<expense><cost_centre>DEV001</cost_centre><total>890.55</total><payment_method>personalcard</payment_method><date>23/07/2018</date><description>testexpenserecord</description></expense>"));
            if (id < 0 || (id > expenses.Count()))
                return Request.CreateResponse(HttpStatusCode.NotFound, "Id Not Found");
            else
                return Request.CreateResponse(HttpStatusCode.OK, expenses[id]);                       
        }

        // POST api/expense
        /// <summary>
        /// This method creates a new expense record with the input supplied
        /// </summary>
        /// <returns>HTTPResponse to indicate the success or failure of the input</returns>   
        public HttpResponseMessage Post([FromBody]string input)
        {
            //console.WriteLine("input=" + input.ToString());            
            int count = expenses.Count();
           
            try
            {
                Expense newExpense = Load(input);    
                
                if (newExpense != null)
                {
                    expenses.Add(newExpense);
                    return Request.CreateResponse(HttpStatusCode.Created, newExpense);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorLog.ToArray());

            }
            catch(Exception ex)
            {
                Error item = new Error(ex);               
                errorLog.Add(item);
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorLog.ToArray());
            }        

        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ExpenseController.Load(string)'
        public Expense Load(string input)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ExpenseController.Load(string)'
        {
            Expense expense = new Expense();
            //Parse input string for XML tags
            XDocument doc;
          
            if (input != null)
            {
                try
                {
                    doc = XDocument.Parse(input);

                    string cost_centre = (string)doc.Descendants("cost_centre").FirstOrDefault();
                    string descElement = (string)doc.Descendants("description").FirstOrDefault();
                    string vendorElement = (string)doc.Descendants("vendor").FirstOrDefault();
                    string payment_method = (string)doc.Descendants("payment_method").FirstOrDefault();
                    string totalElement = (string)doc.Descendants("total").FirstOrDefault();
                    bool totalParseResult = decimal.TryParse(totalElement, out decimal total);
                    string dateElement = (string)doc.Descendants("date").FirstOrDefault();
                    bool dateParseResult = DateTime.TryParse(dateElement, out DateTime date);
                    //DateTime? date = string.IsNullOrWhiteSpace(dateElement)?(DateTime?)null:DateTime.Parse(dateElement);

                    if (totalParseResult == true)
                    {
                        expense.Total = total;
                        if (cost_centre is null || cost_centre == "")
                        {
                            cost_centre = "UNKNOWN";
                        }

                        expense.Cost_Centre = cost_centre;

                        if (dateParseResult == true)
                            expense.Date = date;
                        expense.Id = expenses.Count();
                        expense.Description = descElement;
                        expense.Payment_Method = payment_method;
                    }
                    else
                    {
                        errorLog.Add(new Error(new Exception("total value Missing or Invalid")));
                        expense = null;
                    }
                }
                catch (System.Xml.XmlException xmlex)
                {
                    expense = null;
                    errorLog.Add(new Error(xmlex));
                }
                catch (Exception ex)
                {
                    expense = null;
                    errorLog.Add(new Error(ex));
                }
            }
            else
            {
                errorLog.Add(new Error(new Exception("input is NULL")));
                expense = null;
            }
            return expense;
        }         
           
    }
}

