import { useState } from "react";
import { Command } from "@tauri-apps/plugin-shell";
import { toast } from "sonner";
import { UseFormReturn } from "react-hook-form";
import { z } from "zod";

export function useCreateProject(form: UseFormReturn<z.infer<any>>) {
  const [isCreating, setIsCreating] = useState(false);
  const [proyectProcess, setProyectProcess] = useState(0);

  async function runScript(data: z.infer<any>) {
    const { projectLocation, projectName, items, frameworkVersion } = data;
    const testingProject = items.includes("unitTest");
    const bdProject = items.includes("dataTools");
    const sdkProyect = items.includes("sdk");

    const scriptTemplatePath = "../src/script/install-template.ps1";

    const command = await Command.create("exec-install-template", [
      "-ExecutionPolicy", "Bypass",
      "-File", scriptTemplatePath,
      "-templateSource", `${projectLocation}\\${projectName}`,
      "-projectName", projectName,
      "-framework", frameworkVersion,
      "-unitTest", `${testingProject}`,
      "-proyectDb", `${bdProject}`,
      "-sdk", `${sdkProyect}`,
    ]);

    command.on("close", (data) => {
      setIsCreating(false);
      setProyectProcess(100);

      if (data.code === 1) {
        toast.success("Project created successfully", {
          description: `The project ${projectName} has been created.`,
        });
      } else {
        toast.error("Error", {
          description: `Something went wrong.`,
        });
      }
    });

    command.stdout.on("data", (d) => setProyectProcess(parseInt(d)));

    await command.spawn();
  }

  async function onSubmit(data: z.infer<any>) {
    setIsCreating(true);
    await runScript(data);
  }

  return { onSubmit, isCreating, proyectProcess };
}