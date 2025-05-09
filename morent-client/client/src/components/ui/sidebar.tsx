import React from "react";
import { useTheme } from "next-themes";
import { Link, useLocation } from "wouter";
import { cn } from "@/lib/utils";
import { 
  BarChart3, 
  Car, 
  LayoutDashboard, 
  Receipt, 
  Inbox, 
  Calendar, 
  Settings, 
  HelpCircle, 
  Moon, 
  Sun,
  LogOut
} from "lucide-react";
import { Switch } from "@/components/ui/switch";

const MenuItem = ({ 
  icon, 
  label, 
  href, 
  active = false 
}: { 
  icon: React.ReactNode; 
  label: string; 
  href: string; 
  active?: boolean; 
}) => {
  return (
    <li>
      <div
        className={cn(
          "flex items-center space-x-3 px-4 py-3 rounded-lg text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-slate-700 cursor-pointer",
          active && "bg-primary text-white hover:bg-primary/90 dark:hover:bg-primary/90"
        )}
        onClick={() => {
          window.location.href = href;
        }}
      >
        {icon}
        <span>{label}</span>
      </div>
    </li>
  );
};

const Sidebar = () => {
  const [location] = useLocation();
  const { theme, setTheme } = useTheme();
  const isDarkMode = theme === "dark";

  const toggleDarkMode = () => {
    setTheme(isDarkMode ? "light" : "dark");
  };

  return (
    <aside className="w-64 bg-white dark:bg-slate-800 border-r border-gray-200 dark:border-slate-700 flex flex-col">
      <div className="px-4 py-6">
        <h2 className="text-xs uppercase text-gray-400 dark:text-gray-500 font-medium mb-4 tracking-wider">
          Main Menu
        </h2>
        <nav>
          <ul className="space-y-1">
            <MenuItem
              icon={<LayoutDashboard className="h-5 w-5" />}
              label="Dashboard"
              href="/"
              active={location === "/"}
            />
            <MenuItem
              icon={<Car className="h-5 w-5" />}
              label="Car Rent"
              href="/cars"
              active={location === "/cars"}
            />
            <MenuItem
              icon={<BarChart3 className="h-5 w-5" />}
              label="Insight"
              href="/insight"
              active={location === "/insight"}
            />
            <MenuItem
              icon={<Receipt className="h-5 w-5" />}
              label="Reimburse"
              href="/reimburse"
              active={location === "/reimburse"}
            />
            <MenuItem
              icon={<Inbox className="h-5 w-5" />}
              label="Inbox"
              href="/inbox"
              active={location === "/inbox"}
            />
            <MenuItem
              icon={<Calendar className="h-5 w-5" />}
              label="Calendar"
              href="/calendar"
              active={location === "/calendar"}
            />
          </ul>
        </nav>
      </div>

      <div className="mt-10 px-4 py-6">
        <h2 className="text-xs uppercase text-gray-400 dark:text-gray-500 font-medium mb-4 tracking-wider">
          Preferences
        </h2>
        <nav>
          <ul className="space-y-1">
            <MenuItem
              icon={<Settings className="h-5 w-5" />}
              label="Settings"
              href="/settings"
              active={location === "/settings"}
            />
            <MenuItem
              icon={<HelpCircle className="h-5 w-5" />}
              label="Help & Center"
              href="/help"
              active={location === "/help"}
            />
            <li>
              <div className="flex items-center justify-between px-4 py-3 rounded-lg text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-slate-700">
                <div className="flex items-center space-x-3">
                  {isDarkMode ? (
                    <Moon className="h-5 w-5" />
                  ) : (
                    <Sun className="h-5 w-5" />
                  )}
                  <span>Dark Mode</span>
                </div>
                <Switch
                  checked={isDarkMode}
                  onCheckedChange={toggleDarkMode}
                />
              </div>
            </li>
          </ul>
        </nav>
      </div>

      <div className="mt-auto p-4 border-t border-gray-200 dark:border-slate-700">
        <div 
          className="flex items-center space-x-3 px-4 py-3 rounded-lg text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-slate-700 cursor-pointer"
          onClick={() => {
            window.location.href = "/logout";
          }}
        >
          <LogOut className="h-5 w-5" />
          <span>Log Out</span>
        </div>
      </div>
    </aside>
  );
};

export default Sidebar;
