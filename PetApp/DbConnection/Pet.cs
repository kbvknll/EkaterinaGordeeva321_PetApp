//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PetApp.DbConnection
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pet
    {
        public int id_pet { get; set; }
        public string type { get; set; }
        public string klichka { get; set; }
        public Nullable<int> id_user { get; set; }
    
        public virtual Users Users { get; set; }
    }
}
