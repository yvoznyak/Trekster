// <copyright file="Transaction.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Trekster_app
{
    using System;

    /// <summary>
    /// Transaction class.
    /// </summary>
    public partial class Transaction
    {
        /// <summary>
        /// Gets or sets id properties.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets date properties.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets id of account properties.
        /// </summary>
        public int Idaccount { get; set; }

        /// <summary>
        /// Gets or sets id of currency properties.
        /// </summary>
        public int Idcurrency { get; set; }

        /// <summary>
        /// Gets or sets id of category properties.
        /// </summary>
        public int Idcategory { get; set; }

        /// <summary>
        /// Gets or sets sum properties.
        /// </summary>
        public float Sum { get; set; }

        /// <summary>
        /// Gets or sets note properties.
        /// </summary>
        public string? Note { get; set; }

        /// <summary>
        /// Gets or sets or id account navigation.
        /// </summary>
        public virtual Account IdaccountNavigation { get; set; } = null!;

        /// <summary>
        /// Gets or sets or id category navigation.
        /// </summary>
        public virtual Category IdcategoryNavigation { get; set; } = null!;

        /// <summary>
        /// Gets or sets or id currency navigation.
        /// </summary>
        public virtual Currency IdcurrencyNavigation { get; set; } = null!;
    }
}
