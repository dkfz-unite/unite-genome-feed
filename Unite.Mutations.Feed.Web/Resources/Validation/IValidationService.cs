﻿using FluentValidation;

namespace Unite.Mutations.Feed.Web.Resources.Validation
{
    public interface IValidationService
    {
        bool ValidateParameter<T>(T parameter, IValidator<T> validator, out string errorMessage);
    }
}