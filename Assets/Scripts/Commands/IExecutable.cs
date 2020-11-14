/*
 * IExecutable objects perform logic on a stage.
 */
public interface IExecutable
{
    bool Execute(Stage stage);
    bool Reverse(Stage stage);
}
