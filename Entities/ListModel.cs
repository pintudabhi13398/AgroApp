using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
  
        public class ListModel
        {
            public ListModel()
            { }

            public ListModel(ListModel listModel)
            {
                Id = listModel.Id;
                Name = listModel.Name;
                Value = listModel.Value;
            }

            public long Id { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
        }
    
}
