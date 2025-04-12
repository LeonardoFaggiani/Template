import "./App.css";
import { Button } from "./components/ui/button";
import { ThemeProvider } from "./create-project/components/theme-provider";
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "./components/ui/form";
import { Checkbox } from "./components/ui/checkbox";
import { toast, Toaster } from "sonner";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { CheckedState } from "@radix-ui/react-checkbox";
import { Input } from "./components/ui/input";
import { useState } from "react";
import { Command } from "@tauri-apps/plugin-shell";
import { open } from "@tauri-apps/plugin-dialog";
import { Progress } from "./components/ui/progress"
import { Loader2 } from "lucide-react";

function App() {

  const items = [
    {
      id: "unitTest",
      label: "Unit Tests",
      description: null
    },
    {
      id: "dataTools",
      label: "Database (SSDT)",
      description: null
    },
    {
      id: "sdk",
      label: "SDK",
      description: "Include unit test proyect."
    },
  ] as const;

  const [projectName, setProjectName] = useState<string | null>("");
  const [proyectProcess, setProyectProcess] = useState<number>(0);
  const [isCreating, setIsCreating] = useState(false);

  const handleSelectFolder = async () => {

    const selectedPath = await open({
      directory: true,
      multiple: false,
    });

    if (typeof selectedPath === "string")
        return selectedPath;
  };

  const formSchema = z.object({
    projectName: z
      .string()
      .min(1, "Name is required")
      .max(50, "Please use 20 characters or less for the name."),
  
    projectLocation: z
      .string()
      .min(1, "Please select a location for the project."),
  
    items: z.array(z.string())
  });

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      projectName:"",
      projectLocation:"",
      items: []
    },
  });

  async function onSubmit(data: z.infer<typeof formSchema>) {
    setIsCreating(true); 
    await runCreateProyectScript(data.projectLocation, data.projectName, data.items);
  }

  async function runCreateProyectScript(
    installationTemplatePath: string,
    proyectName: string,
    proyectItems:Array<string>
  ) {

    const scriptTemplatePath = "../src/script/install-template.ps1";

    const testingProject = proyectItems.indexOf("unitTest") != -1
    const bdProject = proyectItems.indexOf("dataTools") != -1
    const sdkProyect = proyectItems.indexOf("sdk") != -1

    const command = await Command.create(
      "exec-install-template",
      [
        "-ExecutionPolicy",
        "Bypass",
        "-File",
        `${scriptTemplatePath}`,
        "-templateSource",
        `${installationTemplatePath}\\${proyectName}`,
        "-projectName",
        `${proyectName}`,
        "-framework",
        `${"net6.0"}`,
        "-unitTest",
        `${testingProject}`,
        "-proyectDb",
        `${bdProject}`,
        "-sdk",
        `${sdkProyect}`
      ],
      { encoding: "utf8" }
    );

    command.on("close", (data) => {
      if (data && data.code != 1) {

        toast.error("Erro!", {          
          description: `Oops something is wrong...`,
          action: {
            label: "Close",        
            onClick: () => console.log("closed"),
          },
        }); 

        console.log("Oops something is wrong...")
      };

      if (data && data.code == 1) {
        setIsCreating(false);
        setProyectProcess(100);

        toast.success("Successfully!", {          
          description: `The proyect ${proyectName} has been created.`,
          action: {
            label: "Close",        
            onClick: () => console.log("closed"),
          },
        }); 

      }
    });

    command.stdout.on("data", (proccess) => {
      setProyectProcess(parseInt(proccess));
    });

    await command.spawn();
  }

  const handleCheckedChange = (
    checked: CheckedState,
    field: any,
    itemId: string
  ) => {
    if (checked) {
      field.onChange([...field.value, itemId]);
    } else {
      field.onChange(field.value?.filter((value: string) => value !== itemId));
    }
  };

  return (
    <ThemeProvider defaultTheme="dark" storageKey="vite-ui-theme">
      <Toaster />
      <div className="h-screen w-screen flex justify-center p-4">
        <div className="p-8 rounded-2xl max-w-md w-full">
          <Form {...form}>
            <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-8">
              <FormField
                control={form.control}
                name="projectName"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel htmlFor="projectName">Proyect name</FormLabel>
                    <FormControl>
                      <Input
                        id="projectName"
                        placeholder="MyProyect"
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

              <FormField
                control={form.control}
                name="projectLocation"
                render={({ field }) => (
                  <FormItem>
                    <FormControl>
                      <>
                        <Button
                          className="w-full"
                          id="projectLocation"
                          type="button"
                          onClick={async () => {
                            const proyectPath = await handleSelectFolder();
                            if (proyectPath) {
                              form.setValue("projectLocation", proyectPath, {
                                shouldDirty: true,
                                shouldTouch: true,
                                shouldValidate: true,
                              });
                            }
                          }}
                        >
                          Proyect location...
                        </Button>
                        {form.watch("projectLocation") && (
                          <p>
                            {form.watch("projectLocation")}\
                            <i className="proyect-name-highlight">
                              {form.watch("projectName")}
                            </i>
                          </p>
                        )}
                      </>
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="items"
                render={() => (
                  <FormItem>
                    <div className="mb-4 mt-5">
                      <FormDescription>
                        Select which project types you want to add to your
                        solution
                      </FormDescription>
                    </div>
                    {items.map((item) => (
                      <FormField
                        key={item.id}
                        control={form.control}
                        name="items"
                        render={({ field }) => (
                          <FormItem className="flex flex-row items-start space-x-3 space-y-0">
                            <FormControl>
                              <Checkbox
                                checked={field.value?.includes(item.id)}
                                onCheckedChange={(checked) =>
                                  handleCheckedChange(checked, field, item.id)
                                }
                              />
                            </FormControl>
                            <FormLabel className="text-sm font-normal">
                              {item.label}
                            </FormLabel>
                            <FormDescription>
                              {item.description}
                            </FormDescription>
                          </FormItem>
                        )}
                      />
                    ))}
                    <FormMessage />
                  </FormItem>
                )}
              />
              <div className="flex items-center space-x-2">
                <Progress className="w-full" hidden={!isCreating} value={proyectProcess} />
              </div>
              <Button type="submit" className="w-full" disabled={isCreating}>
                {isCreating ? (
                  <>
                    <Loader2 className="animate-spin" />
                    Working...
                  </>
                ) : (
                  "Create"
                )}
              </Button>
            </form>
          </Form>
        </div>
      </div>
    </ThemeProvider>
  );
}

export default App;
