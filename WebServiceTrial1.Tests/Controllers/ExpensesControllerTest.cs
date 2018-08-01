using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebServiceTrial1;
using WebServiceTrial1.Controllers;
using WebServiceTrial1.Models;

namespace WebServiceTrial1.Tests.Controllers
{
    [TestClass]
    public class ExpensesControllerTest
    {
        
        public void LoadTestData(ExpenseController expenseController)
        {
            Expense[] localExpense = new Expense[]
            {
                 new Expense { Id = 1, Vendor="ABC", Date=DateTime.Parse("2018-07-07"), Description="Coffee", Payment_Method="Credit-Card",Cost_Centre="CAP01", Total=15.00M},
                 new Expense { Id = 2, Vendor="CAB", Date=DateTime.Parse("2018-07-06"), Description="Tea", Payment_Method="CASH",Cost_Centre="CAP02", Total=25.00M},
                 new Expense { Id = 3, Vendor="BCA", Date=DateTime.Parse("2018-07-05"), Description="Juice", Payment_Method="Debit-Card",Cost_Centre="CAP03", Total=35.00M}
            };
                      
            expenseController.expenses.Add(localExpense[0]);
            expenseController.expenses.Add(localExpense[1]);
            expenseController.expenses.Add(localExpense[2]);
        }

        [TestMethod()]
        public void LoadTestValid1()
        {
            // Arrange
            ExpenseController controller = new ExpenseController();
            string body = "<expense><cost_centre>DEV002</cost_centre><total>890.55</total><payment_method>personalcard</payment_method></expense>";

            // Act
            Expense result = controller.Load(body);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("DEV002", result.Cost_Centre.ToString());
        }

        public void LoadTestValid2()
        {
            // Arrange
            ExpenseController controller = new ExpenseController();
            string body = "<vendor>Viaduct Steakhouse</vendor><description>development team’s project end celebration dinner</description><date>Tuesday 27 April 2017</date >";

            // Act
            Expense result = controller.Load(body);

            // Assert
            Assert.IsNull(result);            
        }

        [TestMethod()]
        public void LoadTestInvalidXML()
        {
            // Arrange
            ExpenseController controller = new ExpenseController();
            string body = "<expense><cost_centre>DEV002<total>890.55</total><payment_method>personalcard</payment_method></expense>";

            // Act
            Expense result = controller.Load(body);

            // Assert
            Assert.IsNull(result);            
        }

        [TestMethod()]
        public void LoadTestMissingTotal()
        {
            // Arrange
            ExpenseController controller = new ExpenseController();
            string body = "<expense><cost_centre>DEV002</cost_centre><payment_method>personalcard</payment_method></expense>";

            // Act
            Expense result = controller.Load(body);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void LoadTestMissingCostCentre()
        {
            // Arrange
            ExpenseController controller = new ExpenseController();
            string body = "<expense><total>890.55</total><payment_method>personalcard</payment_method></expense>";

            // Act
            Expense result = controller.Load(body);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("UNKNOWN", result.Cost_Centre.ToString());
        }

        [TestMethod]
        public void Get()
        {
            // Arrange                                                                                
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new System.Web.Http.Routing.HttpRouteData(httpConfiguration.Routes["DefaultApi"],
            new System.Web.Http.Routing.HttpRouteValueDictionary { { "controller", "Expense" } });
            ExpenseController controller = new ExpenseController()
            {
                Request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:53852/api/Expense/")
                {
                    Properties =
                   {
                    { System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                    { System.Web.Http.Hosting.HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
                     }
                }
            };
            LoadTestData(controller);
            // Act
            var response = controller.Get();

            // Assert
            Assert.AreEqual(response.StatusCode,System.Net.HttpStatusCode.OK);
        }

        [TestMethod]
        public void GetById()
        {
            // Arrange
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new System.Web.Http.Routing.HttpRouteData(httpConfiguration.Routes["DefaultApi"],
            new System.Web.Http.Routing.HttpRouteValueDictionary { { "controller", "Expense" } });
            ExpenseController controller = new ExpenseController()
            {
                Request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:53852/api/Expense/")
                {
                    Properties =
                   {
                    { System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                    { System.Web.Http.Hosting.HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
                     }
                }
            };

            LoadTestData(controller);
            // Act
            var result = controller.Get(0);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public void Post()
        {
            // Arrange
            
           var httpConfiguration = new HttpConfiguration();
           WebApiConfig.Register(httpConfiguration);
           var httpRouteData = new System.Web.Http.Routing.HttpRouteData(httpConfiguration.Routes["DefaultApi"],
           new System.Web.Http.Routing.HttpRouteValueDictionary { { "controller", "Expense" } });
           ExpenseController controller = new ExpenseController()
           {
                Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:53852/api/Expense/")
                {
                   Properties =
                   {
                    { System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                    { System.Web.Http.Hosting.HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
                     }
                    }
            };
           
            // Act
            var response = controller.Post("<expense><cost_centre>DEV002</cost_centre><total>890.55</total><payment_method>personalcard</payment_method><date>23/07/2018</date><description>test expense record</description></expense>");

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.Created, response.StatusCode);            
        }
       
    }
}
