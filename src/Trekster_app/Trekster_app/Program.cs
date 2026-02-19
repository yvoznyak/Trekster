// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Trekster_app
{
    using System;

    /// <summary>
    /// Program Class.
    /// </summary>
    public static class Program
    {
        #pragma warning disable CS8602 // Dereference of a possibly null reference.
        /// <summary>
        /// Logger.
        /// </summary>
        public static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #pragma warning restore CS8602 // Dereference of a possibly null reference.

        /// <summary>
        /// Main Func.
        /// </summary>
        /// <param name="args"> Param. </param>
        [STAThread]
        public static void Main(string[] args)
        {
            Log.Info("App started.");

            var app = new App();
            app.InitializeComponent();
            app.Run();

            Log.Info("App Ended.");
        }
    }
}