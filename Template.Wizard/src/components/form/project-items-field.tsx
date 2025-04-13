import {
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "../../components/ui/form";
import { Checkbox } from "../../components/ui/checkbox";
import { items, formSchema } from "../../lib/form-schema";
import { UseFormReturn } from "react-hook-form";
import { z } from "zod";
type FormSchema = z.infer<typeof formSchema>;

type ProjectItemsFieldProps = {
  form: UseFormReturn<FormSchema>;
};

export function ProjectItemsField({ form }: ProjectItemsFieldProps) {
  return (
    <FormField
      control={form.control}
      name="items"
      render={() => (
        <FormItem>
          <div className="mb-1 mt-1">
            <FormDescription>
              Select which project types you want to add to your solution
            </FormDescription>
          </div>
          <div className="flex flex-col space-y-2">
            {items.map((item) => (
              <FormField
                key={item.id}
                control={form.control}
                name="items"
                render={({ field }) => (
                  <FormItem
                    key={item.id}
                    className="flex flex-row items-start space-x-3 space-y-0"
                  >
                    <FormControl>
                      <Checkbox
                        checked={field.value?.includes(item.id)}
                        onCheckedChange={(checked) => {
                          return checked
                            ? field.onChange([...field.value, item.id])
                            : field.onChange(
                                field.value?.filter(
                                  (value) => value !== item.id
                                )
                              );
                        }}
                      />
                    </FormControl>
                    <FormLabel className="font-normal">{item.label}</FormLabel>
                  </FormItem>
                )}
              />
            ))}
          </div>
          <FormMessage />
        </FormItem>
      )}
    />
  );
}
