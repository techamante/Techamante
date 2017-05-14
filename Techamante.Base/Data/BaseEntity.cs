using System;
using System.ComponentModel.DataAnnotations;


namespace Techamante.Data
{
    public abstract class BaseEntity : Entity
    {
        public int Id { get; set; }

        public Guid EntityGuid { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public DateTimeOffset CreatedDtOffset { get; set; }


    }
}