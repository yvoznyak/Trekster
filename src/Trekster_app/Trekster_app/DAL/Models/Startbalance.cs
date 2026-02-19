// <copyright file="Startbalance.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Trekster_app
{
    /// <summary>
    /// Start balance class.
    /// </summary>
    public partial class Startbalance
    {
        /// <summary>
        /// Gets or sets id properties.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets id of account properties.
        /// </summary>
        public int Idaccount { get; set; }

        /// <summary>
        /// Gets or sets id of currency properties.
        /// </summary>
        public int Idcurrency { get; set; }

        /// <summary>
        /// Gets or sets sum properties.
        /// </summary>
        public float Sum { get; set; }

        /// <summary>
        /// Gets or sets id account navigation.
        /// </summary>
        public virtual Account IdaccountNavigation { get; set; } = null!;

        /// <summary>
        /// Gets or sets id currency navigation.
        /// </summary>
        public virtual Currency IdcurrencyNavigation { get; set; } = null!;
    }
}
