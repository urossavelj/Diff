namespace Tests
{
    public class UnitTests
    {
        [Fact]
        public void IfTwoValidDiffsAreEnteredCorrectDiffsShouldBeReturned()
        {
            var left = "dGhpcyBpcyBqdXN0IGEgdGVzdAp0byB0ZXN0IHNvbWV0aGluZw==";
            var right = "dGhpcyBpcyBqdXN0IGEgdGVzdAp0byBmYXJ0IHNvbWV0aGluZwo=";

            var diff = new Diff.Diff()
            {
                Left = left,
                Right = right
            };

            var result = DiffLogic.FindDifference(diff);

            Assert.NotNull(result);
            Assert.True(result.Count() == 3, "Result does not have 3 differences");
            Assert.True(result.ContainsKey(31) && result.ContainsKey(34) && result.ContainsKey(50));
            Assert.True(result[31] == 2 && result[34] == 1 && result[50] == 1);
        }
    }
}