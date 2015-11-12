// -----------------------------------------------------------------------
//  <copyright file="PutTypeCommand.cs" company="Hibernating Rhinos LTD">
//      Copyright (c) Hibernating Rhinos LTD. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
namespace Raven.Database.TimeSeries.Raft.Commands
{
    public class PutTypeCommand : TimeSeriesCommand
    {
        public string Type { get; set; }
        public string[] Fields { get; set; }
    }
}