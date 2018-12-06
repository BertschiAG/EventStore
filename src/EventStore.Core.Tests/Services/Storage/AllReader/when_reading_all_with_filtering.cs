﻿using System;
using NUnit.Framework;
using EventStore.Core.Data;
using System.Collections.Generic;
using EventStore.Core.Util;

namespace EventStore.Core.Tests.Services.Storage.AllReader
{
    public class when_reading_all_with_filtering : ReadIndexTestScenario
    {
        EventRecord firstEvent;

        protected override void WriteTestScenario()
        {
            this.firstEvent = this.WriteSingleEvent("ES1", 1, new string('.', 3000), eventId: Guid.NewGuid(), eventType: "event-type", retryOnFail: true);
            this.WriteSingleEvent("ES2", 1, new string('.', 3000), eventId: Guid.NewGuid(), eventType: "other-event-type", retryOnFail: true);
            this.WriteSingleEvent("ES3", 1, new string('.', 3000), eventId: Guid.NewGuid(), eventType: "event-type", retryOnFail: true);
            this.WriteSingleEvent("ES4", 1, new string('.', 3000), eventId: Guid.NewGuid(), eventType: "other-event-type", retryOnFail: true);
        }

        [Test]
        public void should_read_only_allowed_events()
        {
            var expectedEventTypes = new StringFilter(new string[] { "event-type" });
            var pos = new TFPos(this.firstEvent.LogPosition, this.firstEvent.LogPosition);
            var result = ReadIndex.ReadAllEventsForward(pos, 10, expectedEventTypes);
            Assert.AreEqual(2, result.Records.Count);
        }
    }
}