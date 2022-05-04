package net.wojtek.zti.api;

import net.wojtek.zti.models.Message;
import net.wojtek.zti.models.User;
import net.wojtek.zti.models.request.CreateMessageRequest;
import net.wojtek.zti.models.response.MessageResponse;
import net.wojtek.zti.services.MessageService;
import net.wojtek.zti.services.UserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.stream.Collectors;

@RestController
@RequestMapping("/api/messages")
public class MessageController {

    @Autowired
    private UserService userService;

    @Autowired
    private MessageService messageService;

    @PostMapping
    @ResponseStatus(code = HttpStatus.CREATED)
    public void createMessage(@RequestBody CreateMessageRequest createMessageRequest){
        User sender = userService.getUserById(createMessageRequest.getSenderId());
        User receiver = userService.getUserById(createMessageRequest.getReceiverId());
        messageService.createMessage(sender, receiver, createMessageRequest.getContent());
    }

    @GetMapping
    @ResponseStatus(HttpStatus.OK)
    public List<MessageResponse> getConversationMessages(@RequestHeader("user-id") int senderId, @RequestParam int receiverId){
        User sender = userService.getUserById(senderId);
        User receiver = userService.getUserById(receiverId);
//        return messageService.getConversationMessages(sender, receiver);
        return messageService.getConversationMessages(sender, receiver).stream()
                .map(MessageResponse::new)
                .collect(Collectors.toList());
//        List<Message> senderMessages = userService.getUserById(senderId).getMessagesSent();
//        senderMessages = messageService.filterMessagesByReceiver(senderMessages, userService.getUserById(receiverId));
    }

}
