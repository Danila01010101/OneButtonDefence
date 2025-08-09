using System.Collections;

public interface IGameInitializerStep
{
    IEnumerator Initialize();
}