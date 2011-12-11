namespace JobZoom.Core
{
    using System;
    using System.Collections.Generic;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public partial class TagAttribute : Attribute
    {
        public TagAttribute()
        {
            this.TagAttribute1 = new HashSet<TagAttribute>();
        }

        public System.Guid TagId { get; set; }
        public string TagName { get; set; }
        public Nullable<short> Weight { get; set; }
        public Nullable<short> Level { get; set; }
        public Nullable<bool> Required { get; set; }
        public string TableReference { get; set; }
        public Nullable<System.Guid> ObjectId { get; set; }
        public Nullable<System.Guid> ParentId { get; set; }
        public string ParentName { get; set; }
        public string Attachment { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Status { get; set; }

        public virtual ICollection<TagAttribute> TagAttribute1 { get; set; }
        public virtual TagAttribute TagAttribute2 { get; set; }
    }
}
