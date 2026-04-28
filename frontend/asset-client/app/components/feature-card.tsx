import { describe } from "node:test";
import { ReactNode } from "react";

interface FeatureDetails {
    icon : ReactNode;
    title : string;
    description : string
}

const FeatureCard = (props : FeatureDetails) => {
    const {icon,title,description} = props;
    return (
        <div className="p-6 rounded-xl border border-zinc-800 bg-zinc-900/50">
            <div className="text-emerald-500 text-2xl mb-4">
              {icon}
            </div>
            <h3 className="font-medium text-lg">{title}</h3>
            <p className="text-zinc-400 text-sm mt-2">
              {description}
            </p>
        </div>
    );
}

export default FeatureCard;