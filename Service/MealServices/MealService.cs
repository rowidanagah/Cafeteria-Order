﻿using CafeteriaOrders.data;
using CafeteriaOrders.logic;
using CafeteriaOrders.logic.DtosModels;
using CafeteriaOrders.logic.Models;
using CafeteriaOrders.UnitOfWork.GenericUnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeteriaOrders.Service
{
    public class MealServices : ImealServices
    {
        Context context;
        UnitOfWorkRepo uof;
        IUnitOfWork _uof;
        public MealServices(Context context)
        {
            _uof = new UnitOfWorkGeneric(context);
            uof = new UnitOfWorkRepo(context);
        }

        public  IEnumerable<Meals> Get() // get all lst
        {
            return  _uof.MealsRepository.Get().ToList();
        }
        public async Task<Meals> Details(int id)
        {
            var meal = await _uof.MealsRepository.GetById(id);
            return meal;
        } // get spedific

        public async Task<IEnumerable<GetMealDto>> HighestRate()
        {
            //mealsViewModel.GroupBy(ml => ml.Id).OrderByDescending(m => m.OverAllRate).Task(7).Select()
            var top7 = uof.meal.viewHighestmeals();
            return top7;
        }
        public async Task<Meals> Add(Meals model)
        {
            try
            {
                var meal = await _uof.MealsRepository.Create(model);
                _uof.Commit();
                return model;
            }
            catch (Exception)
            {

                return null;
            }
           
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                await _uof.MealsRepository.Delete(id);
                _uof.Commit();
                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<Meals> Edit(Meals model)
        {
            try
            {
                await _uof.MealsRepository.Update(model.Id, model);
                _uof.Commit();
                return model;
            }
            catch { return null; }
        }
    }  
}
