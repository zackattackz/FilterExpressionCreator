﻿using FS.FilterExpressionCreator.Demo.Models;
using System.Collections.Generic;

namespace FS.FilterExpressionCreator.Demo.DTOs
{
    /// <summary>
    /// Database query result including execution information.
    /// </summary>
    public class FreelancerDto
    {
        /// <summary>
        /// Data retrieved by database query
        /// </summary>
        public List<Freelancer> Data { get; set; }

        /// <summary>
        /// Count of records return by query
        /// </summary>
        public int FilteredCount { get; set; }

        /// <summary>
        /// Count of records available without filter
        /// </summary>
        /// <autogeneratedoc />
        public int UnfilteredCount { get; set; }

        /// <summary>
        /// Sql command used to query database
        /// </summary>
        public string SqlQuery { get; set; }

        /// <summary>
        /// Filter expression used to query database
        /// </summary>
        public string FilterExpression { get; set; }

        /// <summary>
        /// HTTP query used to retrieve data
        /// </summary>
        public string HttpQuery { get; set; }
    }
}
