namespace Modules.Scenes.MainMenu.Runtime.Choices
{
    public interface IGetResult<out T>
    {
        public T GetResult(int id);
    }
}