﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDM.BLL.Model
{
    public class MyEnums
    {
        public enum enumMaster
        {
            RoleApps
            , WFAction
            , DocumentType
        }

        public enum enumStatus
        {
            SUCCESS,
            ERROR
        }

        public enum actionStatus
        {
            
            APPROVE,
            REJECT,
            ACKNOWLEDGE,
        }

        public enum workflowStatus
        {
            PENDING,
            COMPLETED
        }

    }
}
