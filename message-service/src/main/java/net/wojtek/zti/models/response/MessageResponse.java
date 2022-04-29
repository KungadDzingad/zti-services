package net.wojtek.zti.models.response;

import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import net.wojtek.zti.dao.MessageRepository;
import net.wojtek.zti.models.Message;

import java.time.LocalDate;
import java.time.LocalDateTime;

@NoArgsConstructor
@Getter
@Setter
public class MessageResponse {
    private int id;
    private int senderId;
    private int receiverId;
    private String content;
    private LocalDateTime date;

    public MessageResponse(Message message){
        this.id = message.getId();
        this.senderId = message.getSender().getId();
        this.receiverId = message.getReceiver().getId();
        this.content = message.getContent();
        this.date = message.getDate();
    }
}
