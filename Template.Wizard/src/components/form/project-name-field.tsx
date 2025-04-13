import {
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "../../components/ui/form";
import { Input } from "../../components/ui/input";
import { UseFormReturn } from "react-hook-form";
import { z } from "zod";
import { formSchema } from "../../lib/form-schema";
import { useEffect, useState } from "react";

type FormSchema = z.infer<typeof formSchema>;

type ProjectNameFieldProps = {
  form: UseFormReturn<FormSchema>;
};

export function ProjectNameField({ form }: ProjectNameFieldProps) {
  const [projectName, setProjectName] = useState(
    form.getValues("projectName") ?? ""
  );

  useEffect(() => {
    const subscription = form.watch((values) => {
      if (values.projectName !== undefined) {
        setProjectName(values.projectName);
      }
    });
    return () => subscription.unsubscribe();
  }, [form]);

  return (
    <FormField
      control={form.control}
      name="projectName"
      render={({ field }) => (
        <FormItem>
          <FormLabel htmlFor="projectName">Project Name</FormLabel>
          <FormControl>
            <Input
              id="projectName"
              placeholder="MyProject"
              {...field}
              value={projectName}
              onChange={(e) => {
                field.onChange(e);
                setProjectName(e.target.value);
              }}
            />
          </FormControl>
          <FormMessage />
        </FormItem>
      )}
    />
  );
}
