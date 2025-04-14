import { Form } from "../../components/ui/form";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { formSchema } from "../../lib/form-schema";
import { useCreateProject } from "../../hooks/useCreateProject";
import { ProjectNameField } from "../form/project-name-field";
import { ProjectLocationField } from "../form/project-location-field";
import { FrameworkSelectField } from "../form/framework-select-field";
import { ProjectItemsField } from "../form/project-items-field";
import { SubmitButton } from "../form/submit-button";
import { Progress } from "../ui/progress";

export function FormWrapper() {
  const form = useForm({
    resolver: zodResolver(formSchema),
    defaultValues: {
      projectName: "",
      projectLocation: "",
      items: [],
      frameworkVersion: "",
    },
  });

  const { onSubmit, isCreating, projectProcess } = useCreateProject(form);

  return (
    <div className="h-screen w-screen flex justify-center p-4">
      <div className="p-8 rounded-2xl max-w-md w-full">
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-8">
            <ProjectNameField form={form} />
            <ProjectLocationField form={form} />
            <FrameworkSelectField form={form} />
            <ProjectItemsField form={form} />
            <div className="flex items-center space-x-2">
              <Progress
                className="w-full"
                hidden={!isCreating}
                value={projectProcess}
              />
            </div>
            <SubmitButton isCreating={isCreating} />
          </form>
        </Form>
      </div>
    </div>
  );
}
