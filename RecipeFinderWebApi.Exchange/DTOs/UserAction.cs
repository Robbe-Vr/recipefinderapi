using RecipeFinderWebApi.Exchange.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RecipeFinderWebApi.Exchange.DTOs
{
    public class UserAction : ICountIdentifiedEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CountId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Endpoint { get; set; }
        public string RequestType { get; set; }
        public string Description { get; set; }
        public string RefObjectId { get; set; }
        public string RefObjectName { get; set; }
        public string ActionPerformedOnTable { get; set; }
        public bool Success { get; set; }
    }
}
