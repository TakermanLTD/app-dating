﻿using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Takerman.Dating.Tests.Unit
{
    public class DatesTests(ITestOutputHelper testOutputHelper, TestFixture fixture) : TestBed<TestFixture>(testOutputHelper, fixture)
    {
        [Fact]
        public void Should_ReturnTrue_When_TheTestIsCalled()
        {
            Assert.True(true);
        }
    }
}