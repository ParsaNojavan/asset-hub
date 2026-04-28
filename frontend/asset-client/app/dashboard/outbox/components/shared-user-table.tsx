import DeleteUserButton from "./delete-user-button";

interface Reciver{
      id: string,
      username: string,
      email: string,
      imgUrl: string
    }

export default function SharedUsersTable({Recivers,shareId} : {Recivers:Reciver[],shareId : string}) { 
    return (
        <div className="overflow-x-auto border border-zinc-800 rounded-lg">
            <table className="w-full text-sm">
              <thead className="bg-zinc-900/60 text-table-text-color">
                <tr>
                  <th className="text-left font-medium px-6 py-4">Reciver</th>
                  <th className="text-right font-medium px-6 py-4">Action</th>
                </tr>
              </thead>

              <tbody>
                {Recivers.map((item) => (
                  <tr
                    key={item.id}
                    className="border-t border-zinc-800 hover:bg-zinc-900/40 transition"
                  >
                    {/* Reciver */}
                    <td className="px-6 py-4">
                      <div className="flex items-center gap-3">
                        <img
                          src={`https://localhost:7024/${item.imgUrl}`}
                          alt={item.email}
                          className="w-8 h-8 rounded-full object-cover bg-zinc-800"
                        />
                        <span className="text-[text-color]">{item.email}</span>
                      </div>
                    </td>

                    {/* Action */}
                    <td className="px-6 py-4">
                      <DeleteUserButton userId={item.id} shareId={shareId}/>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
    );
}