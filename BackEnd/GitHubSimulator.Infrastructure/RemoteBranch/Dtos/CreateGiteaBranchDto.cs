using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubSimulator.Infrastructure.RemoteBranch.Dtos
{
    public class CreateGiteaBranchDto
    {
        public string New_Branch_Name { get; set; }
        public string Old_Bracnch_Name { get; set; }
        public string Old_Ref_Name { get; set; }

        public CreateGiteaBranchDto(string new_Branch_Name, string old_Bracnch_Name, string old_Ref_Name) {
            New_Branch_Name = new_Branch_Name;
            Old_Bracnch_Name = old_Bracnch_Name;
            Old_Ref_Name = old_Ref_Name;
        }
        
    }
}
