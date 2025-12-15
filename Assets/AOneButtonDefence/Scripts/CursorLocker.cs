using System;
using UnityEngine;

public class CursorLocker : IDisposable
{
    private IInput input;
    
    public CursorLocker(IInput input)
    {
        this.input = input;
        input.RotateStarted += LockCursor;
        input.RotateEnded += UnlockCursor;
    }
    
    private void LockCursor() => Cursor.lockState = CursorLockMode.Locked;
    
    private void UnlockCursor() => Cursor.lockState = CursorLockMode.None;

    public void Dispose()
    {
        input.RotateStarted -= LockCursor;
        input.RotateEnded -= UnlockCursor;
    }
}