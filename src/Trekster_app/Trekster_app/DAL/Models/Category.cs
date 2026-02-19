// <copyright file="Category.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Trekster_app
{
    using System.Collections.Generic;

    /// <summary>
    /// Category class.
    /// </summary>
    public partial class Category
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Category"/> class.
        /// </summary>
        public Category()
        {
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
        /// Gets or sets type properties.
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets transaction properties.
        /// </summary>
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
