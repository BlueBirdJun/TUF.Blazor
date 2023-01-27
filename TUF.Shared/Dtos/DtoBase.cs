

using Knus.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TUF.Shared.Dtos
{
   
    public enum ContentState
    {
        NotContent, NoAutor, Blind, Delete, Exfire
    }

    
    public class DtoBase<T, S> 
    { 
        public bool Success { get; set; } = false;

        public bool HasAlert { get; set; } = false;
        public bool HasError { get; set; } = false;
        public string Message { get; set; } 
        public List<string> ListMessage { get; set; } = new List<string>();

        public string SystemMessage { get; set; }

        public string Code { get; set; }
        public S InputValue { get; set; }
        public T OutPutValue { get; set; }
        public ContentState ContentState { get; set; }

        public void AlertMessage(string mes)
        {
            this.Message = mes;
            this.Success = false;
            this.HasAlert = true;
        }

        public void ErrorMessage(string mes)
        {
            this.Message = mes;
            this.Success = false;
            this.HasError = true;
        }
        
    }
}
