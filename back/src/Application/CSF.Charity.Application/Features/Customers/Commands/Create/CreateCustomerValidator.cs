﻿using CSF.Charity.Application.Services;
using FluentValidation;
using System;

namespace CSF.Charity.Application.Customers.Commands.Create
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        
        public CreateCustomerCommandValidator(IDateTime dateTime)
        {
            RuleFor(v => v.StateId)
                .NotEmpty();

            RuleFor(v => v.TownshipId)
                .NotEmpty();

            RuleFor(v => v.Firstname)
                .MaximumLength(200)
                .NotEmpty();

            RuleFor(v => v.FirstnameLatin)
                .MaximumLength(200)
                .NotEmpty();

            RuleFor(v => v.BirthPlace)
                .MaximumLength(200)
                .NotEmpty();

            RuleFor(v => v.BirthPlaceLatin)
                .MaximumLength(200)
                .NotEmpty();


            RuleFor(v => v.IdCardNumber)
                .MaximumLength(20)
                .MinimumLength(4)
                .NotEmpty();


            RuleFor(v => v.PhoneNumber)
                .MaximumLength(12)
                .MinimumLength(5)
                .NotEmpty();


            RuleFor(v => v.BirthDate)
                .LessThan(dateTime.Now - TimeSpan.FromDays(GetElligibleCustomerAgeInYears()*365))
                .WithMessage($"Customer must be at least {GetElligibleCustomerAgeInYears()} years old")
                .NotEmpty();
            

            // TODO! add state id verification

        }

        private int GetElligibleCustomerAgeInYears() => 15;
    }
}