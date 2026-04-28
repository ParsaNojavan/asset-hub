import Link from "next/link"

interface PriceDetails {
    title : string;
    price : number;
    description : string;
    features : string[];
}

const PricingCard = (props : PriceDetails) => {
    const {title,price,description,features} = props;
    return (
      <div className="flex justify-center">
          <div className="w-full max-w-sm rounded-xl border border-zinc-800 bg-zinc-900 p-8 text-center">

            <h3 className="text-xl font-semibold">{title} Plan</h3>

            <p className="text-4xl font-bold mt-4 text-emerald-500">
              ${price}
            </p>

            <p className="text-zinc-400 text-sm mt-2">
              {description}
            </p>

            <ul className="mt-6 space-y-3 text-sm text-zinc-300">
              {features.map((feature, index) => {
                return <li key={index}>✔ {feature}</li>
              })}
            </ul>

            <Link
              href="/register"
              className="mt-8 inline-block w-full rounded-lg bg-emerald-500 py-3 text-sm font-medium text-white hover:bg-emerald-600 transition"
            >
              Get Started
            </Link>

            <p className="text-xs text-zinc-500 mt-4">
              More plans coming soon
            </p>

          </div>
        </div>
    )
}

export default PricingCard;