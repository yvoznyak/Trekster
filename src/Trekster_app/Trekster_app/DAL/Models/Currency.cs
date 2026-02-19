// <copyright file="Currency.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Trekster_app
{
    using System.Collections.Generic;

    /// <summary>
    /// Currency class.
    /// </summary>
    public partial class Currency
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Currency"/> class.
        /// </summary>
        public Currency()
        {
            this.Startbalances = new HashSet<Startbalance>();
            this.Transactions = new HashSet<Transaction>();
        }

        /// <summary>
        /// Gets or sets id properties.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name properties.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets start balance properties.
        /// </summary>
        public virtual ICollection<Startbalance> Startbalances { get; set; }

        /// <summary>
        /// Gets or sets transaction properties.
        /// </summary>
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
