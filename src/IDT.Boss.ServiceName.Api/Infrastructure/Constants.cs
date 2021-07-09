using System;

namespace IDT.Boss.ServiceName.Api.Infrastructure
{
    /// <summary>
    /// Some constants for the API.
    /// </summary>
    public readonly struct Constants
    {
        /// <summary>
        /// Name of the API - to use on OpenAPI specification.
        /// </summary>
        public const string ApiName = "Service Name API";

        /// <summary>
        /// Description for the API - to use on OpenAPI specification.
        /// </summary>
        public const string ApiDescription = "Simple service with API";

        /// <summary>
        /// Author of the API.
        /// </summary>
        public const string ApiAuthor = "IDT RCS";

        /// <summary>
        /// Email of teh author (company email).
        /// </summary>
        public const string ApiAuthorEmail = "rcs@idt.net";

        /// <summary>
        /// Copyright for the Swagger.
        /// </summary>
        public static readonly string Copyright = $"Copyright (c) {DateTime.Today.Year}, IDT";

        /// <summary>
        /// Url to the company main web site.
        /// </summary>
        public const string IdtUrl = "https://www.idt.net/";
    }
}