﻿using System;
using System.Collections.Generic;
using System.Linq;
using VMS.TPS.Common.Model.API;

public static class Extensions
{
    public static IEnumerable<PlanningItem> GetPlanSetupsAndSums(this Course course)
    {
        var plans = new List<PlanningItem>();

        if (course.PlanSetups != null)
            plans.AddRange(course.PlanSetups);

        if (course.PlanSums != null)
            plans.AddRange(course.PlanSums);

        return plans;
    }

    public static Course GetCourse(this PlanningItem plan)
    {
        switch (plan)
        {
            case PlanSetup planSetup: return planSetup.Course;
            case PlanSum planSum: return planSum.Course;
        }

        throw new InvalidOperationException("Unknown PlanningItem type.");
    }

    public static string GetStructureSetId(this PlanningItem plan)
    {
        try
        {
            switch (plan)
            {
                case PlanSetup planSetup: return planSetup.StructureSet.Id;
                case PlanSum planSum: return planSum.StructureSet.Id;
            }

            throw new InvalidOperationException("Unknown PlanningItem type.");
        }
        catch
        {
            return "";
        }
    }

    public static string GetPlanImageId(this PlanningItem plan)
    {
        try
        {
            switch (plan)
            {
                case PlanSetup planSetup: return planSetup.StructureSet.Image.Id;
                case PlanSum planSum: return planSum.StructureSet.Image.Id;
            }

            throw new InvalidOperationException("Unknown PlanningItem type.");
        }
        catch
        {
            return "";
        }
    }
    public static DateTime GetPlanImageCreation(this PlanningItem plan)
    {
        try
        {
            switch (plan)
            {
                case PlanSetup planSetup: return (DateTime) planSetup.StructureSet.Image.CreationDateTime;
                case PlanSum planSum: return (DateTime) planSum.StructureSet.Image.CreationDateTime;
            }

            throw new InvalidOperationException("Unknown PlanningItem type.");
        }
        catch
        {
            return DateTime.Now;
        }
    }

    public static PlanningItem GetPlanningItem(Patient patient, string courseId, string planId)
    {
        var course = GetCourse(patient, courseId);
        switch (course.GetPlanSetupsAndSums().FirstOrDefault(x => x.Id == planId))
        {
            case PlanSetup planSetup: return planSetup;
            case PlanSum planSum: return planSum;
        }
        throw new InvalidOperationException("Unknown PlanningItem type.");
    }

    public static DateTime GetCreationDateTime(this PlanningItem plan)
    {
        switch (plan)
        {
            case PlanSetup planSetup: return (DateTime)planSetup.CreationDateTime;
            case PlanSum planSum: return (DateTime)planSum.PlanSetups.LastOrDefault().CreationDateTime;
        }
        throw new InvalidOperationException("Unknown PlanningItem type.");
    }

    public static string GetPlanType(this PlanningItem plan)
    {
        switch (plan)
        {
            case PlanSetup planSetup: return "Plan";
            case PlanSum planSum: return "PlanSum";
        }
        throw new InvalidOperationException("Unknown PlanningItem type.");
    }

    public static string GetFractionation(this PlanningItem plan)
    {
        switch (plan)
        {
            case PlanSetup planSetup: return "  -  " + planSetup.DosePerFraction.ValueAsString + " " + planSetup.DosePerFraction.UnitAsString + " x " + planSetup.NumberOfFractions + " to " + planSetup.TotalDose.ValueAsString + " " + planSetup.TotalDose.UnitAsString;
            case PlanSum planSum: return "";
        }
        throw new InvalidOperationException("Unknown PlanningItem type.");
    }

    private static Course GetCourse(Patient patient, string courseId) =>
        patient?.Courses?.FirstOrDefault(x => x.Id == courseId);

    public static Structure GetStructure(PlanningItem plan, string structureCode) =>
        plan?.StructureSet?.Structures?.FirstOrDefault(x => x.StructureCodeInfos.FirstOrDefault().Code == structureCode);
}