﻿using MainApplication.Objects;

namespace MainApplication.Services.Saves;

public class CompleteSave : ISave
{
    public Save Save { get; set; }

    public CompleteSave(Save save)
    {
        Save = save;
    }

    public bool RunSave()
    {
        throw new NotImplementedException();
    }
}