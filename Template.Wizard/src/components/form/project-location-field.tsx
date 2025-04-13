import {
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "../../components/ui/form";
import { Button } from "../../components/ui/button";
import { FolderOpen } from "lucide-react";
import { UseFormReturn } from "react-hook-form";
import { z } from "zod";
import { formSchema } from "../../lib/form-schema";
import { useEffect, useMemo, useState } from "react";
import { openFolder } from "../../utils/select-folder";

type FormSchema = z.infer<typeof formSchema>;

type ProjectLocationFieldProps = {
  form: UseFormReturn<FormSchema>;
};

export function ProjectLocationField({ form }: ProjectLocationFieldProps) {
  const [projectLocation, setProjectLocation] = useState(
    form.getValues("projectLocation") ?? ""
  );

  const [projectName, setProjectName] = useState(
    form.getValues("projectName") ?? ""
  );

  const renderPath = useMemo(() => {
    if (!projectLocation) return "Select a folder";

    if (projectLocation && !projectName) return projectLocation;

    if (projectLocation && projectName) {
      return (
        <p className="text-sm text-muted-foreground">
          {projectLocation}\
          <span className="proyect-name-highlight">{projectName}</span>
        </p>
      );
    }
  }, [projectLocation, projectName]);

  useEffect(() => {
    const subscription = form.watch((values) => {
      if (values.projectLocation !== undefined) {
        setProjectLocation(values.projectLocation);
      }

      if (values.projectName !== undefined) {
        setProjectName(values.projectName);
      }
    });
    return () => subscription.unsubscribe();
  }, [form]);

  return (
    <FormField
      control={form.control}
      name="projectLocation"
      render={({ field }) => (
        <FormItem>
          <FormLabel>Project Location</FormLabel>
          <FormControl>
            <div className="space-y-1">
              <Button
                type="button"
                variant="outline"
                className="w-full justify-start text-left font-normal"
                onClick={async () => {
                  const selected = await openFolder();
                  if (selected) {
                    field.onChange(selected);
                    setProjectLocation(selected);
                  }
                }}
              >
                <FolderOpen className="mr-2 h-4 w-4" />
                {renderPath}
              </Button>
            </div>
          </FormControl>
          <FormMessage />
        </FormItem>
      )}
    />
  );
}
