using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebServiceTrial1.Models
{
    /// <summary>
    /// Model Expense data structure
    /// </summary>
    [KnownType(typeof(Expense))]
    [DataContract(IsReference = true)]      
    public class Expense
    {  
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Vendor
        /// </summary>
        [DataMember]
        public string Vendor { get; set; }

        /// <summary>
        /// Date
        /// </summary>
        [DataMember]
        public DateTime Date { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Payment_Method 
        /// </summary>
        [DataMember]
        public string Payment_Method { get; set; }

        /// <summary>
        /// Cost_Centre defaults to "UNKNOWN"
        /// </summary>
        [DataMember]
        public string Cost_Centre { get; set; }

        /// <summary>
        /// Total amount mandatory
        /// </summary>
        [DataMember]        
        public decimal Total { get; set; }       
    }
}