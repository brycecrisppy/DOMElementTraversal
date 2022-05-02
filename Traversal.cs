using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

public static class Traversals
{
    public static List<string> GetDescendantPaths(this IWebElement element,
        IWebDriver driver,
        List<string>? path = null)
    {
        var children = WaitForChildren(element, driver);

        path ??= new List<string> {element.TagName};

        if (children.Count == 0)
            return path;

        foreach (var child in children)
        {
            path.Add(child.TagName);
            
            Console.WriteLine(ConvertPathListToString(path));

            path = GetDescendantPaths(child, driver, path);
            path.RemoveAt(path.Count - 1);
        }

        return path;
    }

    public static List<string> GetPathToTargetElementTag(this IWebElement element,
        string targetElementTag,
        IWebDriver driver,
        List<string>? path = null)
    {
        var children = WaitForChildren(element, driver);

        path ??= new List<string> {element.TagName};

        if (children.Count == 0)
            return path;
        
        foreach (var child in children)
        {
            path.Add(child.TagName);

            if (child.TagName == targetElementTag)
                return path;

            path = GetDescendantPaths(child, driver, path);

            if (path.Contains(targetElementTag))
                return path;
            
            path.RemoveAt(path.Count - 1);
        }

        return path;
    }

    public static List<IWebElement> WaitForChildren(IWebElement element, IWebDriver driver)
    {
        return new WebDriverWait(driver, TimeSpan.FromSeconds(5))
            .Until(drv => element.FindElements(By.XPath("./*"))).ToList();
    }

    public static string ConvertPathListToString(List<string> path)
    {
        var newPath = "";
        path.ForEach(p => newPath += $"{p}/");
        return newPath;
    }
}
