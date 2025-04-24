namespace CarRental.API.Middleware;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using SixLabors.ImageSharp;

public class ImageValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ImageValidationOptions _options;
    public ImageValidationMiddleware(RequestDelegate next, ImageValidationOptions options)
    {
        _next = next;
        _options = options;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        // Check if the request is for uploading an image
        if (IsImageUploadRequest(context.Request) && context.Request.HasFormContentType)
        {
            // Check if the form has files
            if (context.Request.Form.Files.Count > 0)
            {
                foreach (var file in context.Request.Form.Files)
                {
                    var validationErrors = ValidateImage(file, _options);
                    
                    if (validationErrors.Any())
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        context.Response.ContentType = "application/json";
                        
                        var response = new
                        {
                            success = false,
                            errors = validationErrors
                        };
                        
                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                        return;
                    }
                }
            }
        }
        // Continue processing the request
        await _next(context);
    }
    private bool IsImageUploadRequest(HttpRequest request)
    {
        // Paths that we know are for uploading images
        var imagePaths = new[]
        {
            "/api/cars/", // Car image uploads
            "/api/users/" // User profile image uploads
        };
        return request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase) &&
               imagePaths.Any(path => request.Path.Value.Contains(path)) &&
               (request.Path.Value.Contains("/images") || request.Path.Value.Contains("/profile-image"));
    }
    private List<string> ValidateImage(IFormFile file, ImageValidationOptions options)
    {
        var errors = new List<string>();
        // Check file size
        if (file.Length > options.MaxFileSize)
        {
            errors.Add($"The file size exceeds the limit of {options.MaxFileSize / (1024 * 1024)}MB");
        }
        // Check content type
        if (!options.AllowedContentTypes.Contains(file.ContentType))
        {
            errors.Add($"The file type '{file.ContentType}' is not allowed. Allowed types: {string.Join(", ", options.AllowedContentTypes)}");
        }
        
        // Check file extension
        var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
        if (string.IsNullOrEmpty(extension) || !options.AllowedExtensions.Contains(extension))
        {
            errors.Add($"The file extension '{extension}' is not allowed. Allowed extensions: {string.Join(", ", options.AllowedExtensions)}");
        }
        
        // Check for empty files
        if (file.Length == 0)
        {
            errors.Add("The file is empty.");
            return errors; // Return early as we can't check dimensions of empty file
        }
        
        // Check image dimensions if configured
        if (options.ValidateDimensions)
        {
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    using (var image = Image.Load(stream))
                    {
                        // Check minimum dimensions
                        if (image.Width < options.MinWidth || image.Height < options.MinHeight)
                        {
                            errors.Add($"Image dimensions are too small. Minimum allowed: {options.MinWidth}x{options.MinHeight}");
                        }
                        
                        // Check maximum dimensions
                        if (image.Width > options.MaxWidth || image.Height > options.MaxHeight)
                        {
                            errors.Add($"Image dimensions are too large. Maximum allowed: {options.MaxWidth}x{options.MaxHeight}");
                        }
                        
                        // Check aspect ratio if required
                        if (options.EnforceAspectRatio)
                        {
                            double actualRatio = (double)image.Width / image.Height;
                            double expectedRatio = (double)options.AspectRatioWidth / options.AspectRatioHeight;
                            
                            // Allow for a small tolerance in aspect ratio comparison
                            const double tolerance = 0.01;
                            if (Math.Abs(actualRatio - expectedRatio) > tolerance)
                            {
                                errors.Add($"Image aspect ratio should be {options.AspectRatioWidth}:{options.AspectRatioHeight}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add($"Failed to validate image dimensions: {ex.Message}");
            }
        }
        
        return errors;
    }
}

/// <summary>
/// Configuration options for the image validation middleware
/// </summary>
public class ImageValidationOptions
{
    /// <summary>
    /// Maximum file size in bytes
    /// </summary>
    public long MaxFileSize { get; set; } = 5 * 1024 * 1024; // 5MB default
    
    /// <summary>
    /// List of allowed content types (MIME types)
    /// </summary>
    public List<string> AllowedContentTypes { get; set; } = new List<string>
    {
        "image/jpeg",
        "image/png",
        "image/gif"
    };
    
    /// <summary>
    /// List of allowed file extensions (including the dot)
    /// </summary>
    public List<string> AllowedExtensions { get; set; } = new List<string>
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".gif"
    };
    
    /// <summary>
    /// Whether to validate the image dimensions
    /// </summary>
    public bool ValidateDimensions { get; set; } = true;
    
    /// <summary>
    /// Minimum allowed width in pixels
    /// </summary>
    public int MinWidth { get; set; } = 100;
    
    /// <summary>
    /// Minimum allowed height in pixels
    /// </summary>
    public int MinHeight { get; set; } = 100;
    
    /// <summary>
    /// Maximum allowed width in pixels
    /// </summary>
    public int MaxWidth { get; set; } = 4000;
    
    /// <summary>
    /// Maximum allowed height in pixels
    /// </summary>
    public int MaxHeight { get; set; } = 4000;
    
    /// <summary>
    /// Whether to enforce a specific aspect ratio
    /// </summary>
    public bool EnforceAspectRatio { get; set; } = false;
    
    /// <summary>
    /// Aspect ratio width component (e.g., 16 for 16:9)
    /// </summary>
    public int AspectRatioWidth { get; set; } = 1;
    
    /// <summary>
    /// Aspect ratio height component (e.g., 9 for 16:9)
    /// </summary>
    public int AspectRatioHeight { get; set; } = 1;
}