/*
 * ITestable objects are tests for levels.
 * The level should first build the test, followed by
 * running all of its commands, then finally checking the test.
 */
public interface ITestable
{
    void Build(Level level);
    bool Check(Level level);
}
