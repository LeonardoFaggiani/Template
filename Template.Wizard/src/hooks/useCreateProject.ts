import { useState } from "react";
import { Command, TerminatedPayload } from "@tauri-apps/plugin-shell";
import { toast } from "sonner";
import { z } from "zod";
import { ErrorMessageMappings } from "../lib/utils";
import { resolveResource  } from "@tauri-apps/api/path";

export function useCreateProject() {
  const [isCreating, setIsCreating] = useState(false);
  const [projectProcess, setProjectProcess] = useState(0);

  async function runScript(data: z.infer<any>) {
    const { projectLocation, projectName, items, frameworkVersion, features } = data;
    const testingProject = items.includes("unitTest");
    const bdProject = items.includes("dataTools");
    const sdkProject = items.includes("sdk");
    const swagger = features.includes("swagger");
    const healthChecks = features.includes("healthChecks");

    const fullScriptPath = await resolveResource("scripts/install-template.ps1");

    const command = await Command.create("exec-install-template", [
      "-ExecutionPolicy", "Bypass",
      "-File", fullScriptPath,
      "-templateSource", `${projectLocation}/${projectName}`,
      "-projectName", projectName,
      "-framework", frameworkVersion,
      "-unitTest", `${testingProject}`,
      "-projectDb", `${bdProject}`,
      "-sdk", `${sdkProject}`,
      "-swagger", `${swagger}`,
      "-healthChecks", `${healthChecks}`
    ]);

    command.on("close", (data:TerminatedPayload) => {
      setIsCreating(false);
      setProjectProcess(100);

      if (data.code === 1) {
        toast.success("Project created successfully", {
          description: `The project ${projectName} has been created.`,
          action: () => {}
        });
      } else {
        toast.error("Error", {
          description: ErrorMessageMappings(data.code),
        });
      }
    });

    command.stdout.on("data", (d) => setProjectProcess(parseInt(d)));

    await command.spawn();
  }

  async function onSubmit(data: z.infer<any>) {
    setIsCreating(true);
    await runScript(data);
  }

  return { onSubmit, isCreating, projectProcess };
}