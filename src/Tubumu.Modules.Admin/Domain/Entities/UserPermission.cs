﻿using System;

namespace Tubumu.Modules.Admin.Domain.Entities
{
    public partial class UserPermission
    {
        public int UserId { get; set; }
        public Guid PermissionId { get; set; }

        public virtual Permission Permission { get; set; }
        public virtual User User { get; set; }
    }
}
