﻿using BethanysPieShopHRM.App.Components;
using BethanysPieShopHRM.App.Services;
using BethanysPieShopHRM.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BethanysPieShopHRM.App.Pages
{
    public partial class EmployeeOverview
    {
        public IEnumerable<Employee> Employees { get; set; }

        [Inject]
        public IEmployeeDataService EmployeeDataService { get; set; }

        protected AddEmployeeDialog AddEmployeeDialog { get; set; }

        [CascadingParameter]
        Task<AuthenticationState> authenticationStateTask { get; set; }


        protected override async Task OnInitializedAsync()
        {
            Employees = (await EmployeeDataService.GetAllEmployees()).ToList();
        }

        protected async Task QuickAddEmployee()
        {
            var authenticationState = await authenticationStateTask;
            if (authenticationState.User.Identity.Name == "Kevin")
            {
                AddEmployeeDialog.Show();
            }
        }

        public async void AddEmployeeDialog_OnDialogClose()
        {
            Employees = (await EmployeeDataService.GetAllEmployees()).ToList();
            StateHasChanged();
        }
    }
}
