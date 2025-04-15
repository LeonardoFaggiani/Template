import { checkIfProjectExists } from "../utils/select-folder";
import { z } from "zod";

export const formSchema = z.object({
  projectName: z
    .string()
    .min(1, "Project name is required")
    .max(50, "Please use 20 characters or less for the name."),
  projectLocation: z
    .string()
    .min(1, "Please select a location for the project."),
  items: z.array(z.string()),
  frameworkVersion: z.string().min(1, "Framework is required"),
  features: z.array(z.string()),
}).superRefine(async (data, ctx) => {
    
    const exists = await checkIfProjectExists(data.projectLocation, data.projectName);

    if (exists) {      
      ctx.addIssue({
        code: z.ZodIssueCode.custom,
        path: ["projectName"],
        message:
          "A project with that name already exists in the selected location.",
      });
    }  

});

export const frameworks = [
  { value: "net6.0", label: "NET 6" },
  { value: "net7.0", label: "NET 7" },
  { value: "net8.0", label: "NET 8" },
];

export const projects = [
  { id: "unitTest", label: "Unit Tests" },
  { id: "dataTools", label: "Database (SSDT)" },
  { id: "sdk", label: "SDK" },
]

export const features = [
  { id: "swagger", label: "Swagger" },
  { id: "healthChecks", label: "HealthChecks" }
]
