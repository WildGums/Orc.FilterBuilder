// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestEntity.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace FilterBuilder.Example.Models
{
    using System;
    using Catel.Data;

    public class TestEntity : ModelBase
    {
        #region Properties
        public string FirstName { get; set; }
        public int Age { get; set; }
        public int? Id { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public bool IsActive { get; set; }
        public TimeSpan Duration { get; set; }
        public decimal Price { get; set; }
        public decimal? NullablePrice { get; set; }
        #endregion
    }
}