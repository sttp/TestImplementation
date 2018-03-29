﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec
{
    public class CommandMetadataRequestFailed : CommandBase
    {
        public readonly string Reason;
        public readonly string Details;

        public CommandMetadataRequestFailed(string reason, string details)
            : base("MetadataRequestFailed")
        {
            Reason = reason;
            Details = details;
        }

        public CommandMetadataRequestFailed(SttpMarkupReader reader)
              : base("MetadataRequestFailed")
        {
            var element = reader.ReadEntireElement();

            Reason = (string)element.GetValue("Reason");
            Details = (string)element.GetValue("Details");


            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
            writer.WriteValue("Reason", Reason);
            writer.WriteValue("Details", Details);
        }
    }
}