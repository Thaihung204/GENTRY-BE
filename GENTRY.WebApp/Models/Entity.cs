using GENTRY.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
namespace GENTRY.WebApp.Models
{
    public abstract class Entity<T> : IEntity<T>
    {
        /// <summary>
        /// The Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; }
        object IEntity.Id
        {
            get => this.Id;
            set => this.Id = (T)value;
        }

        [TriggerProperty(DisplayName = "Created Date")]
        [DataType(DataType.DateTime)]
        [Column(TypeName = "datetime2")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [TriggerProperty(DisplayName = "Modified Date")]
        [DataType(DataType.DateTime)]
        [Column(TypeName = "datetime2")]
        public DateTime? ModifiedDate { get; set; } = null;

        [MaxLength(100)]
        public string? CreatedBy { get; set; } = null;

        [MaxLength(100)]
        public string? ModifiedBy { get; set; } = null;

        //[Timestamp]
        //public byte[]? Version { get; set; } = null;

        //[Display(Description = "For internal use")]
        //[Column(TypeName = "nvarchar(max)")]
        //public string? PropertiesJson { get; set; } = null;

        //[NotMapped]
        //public ExpandoObject CustomProperties
        //{
        //    get => string.IsNullOrEmpty(this.PropertiesJson) ? new ExpandoObject() : JsonConvert.DeserializeObject<ExpandoObject>(this.PropertiesJson);
        //    set => this.PropertiesJson = JsonConvert.SerializeObject(value);
        //}

        //public void SetCustomProperty<TType>(TType customPropertyId, object value)
        //{
        //    IDictionary<string, object> memberCustomProperties = this.CustomProperties;
        //    memberCustomProperties[customPropertyId.ToString()] = value;
        //    this.CustomProperties = (ExpandoObject)memberCustomProperties;
        //}

        //public T GetCustomProperty<T>(object customPropertyId)
        //{
        //    IDictionary<string, object> properties = this.CustomProperties;
        //    if (properties != null && properties.ContainsKey(customPropertyId.ToString()))
        //    {
        //        var value = properties[customPropertyId.ToString()].ToString();
        //        return JsonConvert.DeserializeObject<T>(value);
        //}

        // Return default/null value for T
        //    return default(T);
        //}
    }
}
